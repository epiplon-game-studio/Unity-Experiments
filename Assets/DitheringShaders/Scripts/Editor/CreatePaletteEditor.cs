using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatePalette))]
public class CreatePaletteEditor : Editor {

	private const int ColorSquares = 16;
	private const int MaxColors = 256;

	private CreatePalette script;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		script = (CreatePalette)target;

		if (GUILayout.Button("Create color palette from .PNG"))
			CreatePalette();
	}

	/// <summary>
	///  Creates and saves a palette image from the unique colors in another image.
	///  
	///  The palette image structure:
	/// 
	///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the third mix color
	///  |__|__|__|__|__|__|__|__ 
	///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the second mix color
	///  |__|__|__|__|__|__|__|__ 
	///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the first mix color
	///  |__|__|__|__|__|__|__|__ 
	/// 
	///  Each horizontal square has a fixed Red component: 0/15 to 15/15.
	///  The blue component increases from 0 to 1 horizontally over a square.
	///  The green component increases from 0 to 1 vertically over a square.
	/// 
	///  The palette image is used in the shaders to convert a truecolor to several dithered colors.
	///  The truecolor RGB components points to N colors in the palette, one color per row of squares. 
	///  The N colors are mixed together in a dithering pattern to produce a close approximation of the original truecolor.
	///  
	///  The steps to create the palette image:
	/// 
	///	 1. Load the color image to a Texture2D
	///	 2. Create a list of all the unique colors in the color image
	///	 3. Create the palette image
	///	 4. a) Loop through each pixel in the palette image and determine the truecolor for that pixel
	///		b) Device a mixing plan to achieve the truecolor
	///		c) Save the N colors in the square column
	///  5. Save the palette image
	/// </summary>
	private void CreatePalette() {
		if (script.MixedColorCount < 1) return;

		// Load the color image to a Texture2D
		Texture2D colorTexture = LoadTexture();
		if (colorTexture == null) return;

		// Create a list of all the unique colors in the color image
		List<Color> paletteColors = new List<Color>();
		bool proceed = false;
		for (int x = 0; x < colorTexture.width; x++)
			for (int y = 0; y < colorTexture.height; y++)
				if (!paletteColors.Contains(colorTexture.GetPixel(x, y))) {
					paletteColors.Add(colorTexture.GetPixel(x, y));
					if (paletteColors.Count > MaxColors && !proceed) {
						proceed = EditorUtility.DisplayDialog("Error", "Source image contains more than " + MaxColors + " colors. Continuing may lock up Unity for a long time", "Continue", "Stop");
						if (!proceed)
							return;
					}
				}

		uint[] palette = new uint[paletteColors.Count];
		for (int i = 0; i < paletteColors.Count; i++)
			palette[i] = ColorToInt(paletteColors[i]);
		MixingPlanner mixingPlanner = new MixingPlanner(palette);

		// Create the palette image
		int height = (int)Math.Pow(2, Math.Ceiling(Math.Log(ColorSquares * script.MixedColorCount - 1) / Math.Log(2)));
		Texture2D paletteTexture = new Texture2D(ColorSquares * ColorSquares, height, TextureFormat.RGB24, false);
		paletteTexture.name = colorTexture.name + "_pal_" + script.MixedColorCount;

		// Loop through each pixel in the palette image and determine the target color for that pixel
		for (int x = 0; x < ColorSquares * ColorSquares; x++)
			for (int y = 0; y < ColorSquares; y++) {
				byte r = (byte)((float)(x / ColorSquares) / (ColorSquares - 1) * 255);
				byte g = (byte)((float)y / (ColorSquares - 1) * 255);
				byte b = (byte)(((float)x % ColorSquares) / (ColorSquares - 1) * 255);
				uint targetColor = (uint)((r << 16) + (g << 8) + b);

				// Device a mixing plan to achieve the truecolor
				uint[] mixingPlan = mixingPlanner.DeviseBestMixingPlan(targetColor, (uint)script.MixedColorCount);

				// Save the N colors in the square column
				for (int c = 0; c < script.MixedColorCount; c++)
					paletteTexture.SetPixel(x, y + (script.MixedColorCount - c - 1) * ColorSquares, paletteColors[(int)mixingPlan[c]]);
			}

		// Save the palette image
		SaveTexture(paletteTexture);
	}

	/// <summary>
	///  Opens a file dialog and loads a .png image to a Texture2D.
	/// </summary>
	private Texture2D LoadTexture() {
		string path = EditorUtility.OpenFilePanel("Select your .PNG color image", "", "png");
		if (path.Length == 0) return null;

		Texture2D texture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
		texture.name = Path.GetFileNameWithoutExtension(path);

		new WWW("file://" + path).LoadImageIntoTexture(texture);

		return texture;
	}

	/// <summary>
	///  Opens a file dialog and saves a Texture2D to a .png image.
	/// </summary>
	private void SaveTexture(Texture2D texture) {
		string path = EditorUtility.SaveFilePanel("Save your new .PNG palette image", "", texture.name + ".png", "png");

		if (path.Length == 0) return;

		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}

	private static uint ColorToInt(Color color) {
		return ((uint)(color.r * 255) << 16) + ((uint)(color.g * 255) << 8) + (uint)(color.b * 255);
	}

	/// <summary>
	///  The mixing planner is based on algorithms and code by Joel Yliluoma.
	///  http://bisqwit.iki.fi/story/howto/dither/jy/
	/// </summary>
	private class MixingPlanner {

		private const double Gamma = 2.2;

		private uint[] palette;
		private uint[] luminance;
		private double[,] gammaCorrect;

		private double GammaCorrect(double v) {
			return Math.Pow(v, Gamma);
		}
		private double GammaUncorrect(double v) {
			return Math.Pow(v, 1.0 / Gamma);
		}

		public MixingPlanner(uint[] palette) {
			this.palette = palette;
			luminance = new uint[palette.Length];
			gammaCorrect = new double[palette.Length, 3];

			for (int i = 0; i < palette.Length; i++) {
				byte r = (byte)((palette[i] >> 16) & 0xff);
				byte g = (byte)((palette[i] >> 8) & 0xff);
				byte b = (byte)(palette[i] & 0xff);

				luminance[i] = (uint)(r * 299 + g * 587 + b * 114);

				gammaCorrect[i, 0] = GammaCorrect(r / 255.0);
				gammaCorrect[i, 1] = GammaCorrect(g / 255.0);
				gammaCorrect[i, 2] = GammaCorrect(b / 255.0);
			}
		}

		private double ColorCompare(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2) {
			double luma1 = (r1 * 299 + g1 * 587 + b1 * 114) / (255.0 * 1000);
			double luma2 = (r2 * 299 + g2 * 587 + b2 * 114) / (255.0 * 1000);
			double lumadiff = luma1 - luma2;
			double diffR = (r1 - r2) / 255.0, diffG = (g1 - g2) / 255.0, diffB = (b1 - b2) / 255.0;
			return (diffR * diffR * 0.299 + diffG * diffG * 0.587 + diffB * diffB * 0.114) * 0.75 + lumadiff * lumadiff;
		}

		public uint[] DeviseBestMixingPlan(uint targetColor, uint colorCount) {
			byte[] inputRgb = new byte[] { 
				(byte)((targetColor >> 16) & 0xff), 
				(byte)((targetColor >> 8) & 0xff), 
				(byte)(targetColor & 0xff) 
			};

			uint[] mixingPlan = new uint[colorCount];

			if (palette.Length == 2) {
				// Use an alternative planning algorithm if the palette only has 2 colors
				uint[] soFar = new uint[] { 0, 0, 0 };

				uint proportionTotal = 0;
				while (proportionTotal < colorCount) {
					uint chosenAmount = 1;
					uint chosen = 0;

					uint maxTestCount = Math.Max(1, proportionTotal);

					double leastPenalty = -1;
					for (uint i = 0; i < palette.Length; ++i) {
						uint color = palette[i];
						uint[] sum = new uint[] { soFar[0], soFar[1], soFar[2] };
						uint[] add = new uint[] { color >> 16, (color >> 8) & 0xff, color & 0xff };
						for (uint p = 1; p <= maxTestCount; p *= 2) {
							for (uint c = 0; c < 3; ++c) sum[c] += add[c];
							for (uint c = 0; c < 3; ++c) add[c] += add[c];
							uint t = proportionTotal + p;
							uint[] test = new uint[] { sum[0] / t, sum[1] / t, sum[2] / t };
							double penalty = ColorCompare((byte)inputRgb[0], (byte)inputRgb[1], (byte)inputRgb[2],
														  (byte)test[0], (byte)test[1], (byte)test[2]);
							if (penalty < leastPenalty || leastPenalty < 0) {
								leastPenalty = penalty;
								chosen = i;
								chosenAmount = p;
							}
						}
					}
					for (uint p = 0; p < chosenAmount; ++p) {
						if (proportionTotal >= colorCount) break;
						mixingPlan[proportionTotal++] = chosen;
					}

					uint newColor = palette[chosen];
					uint[] palcolor = new uint[] { newColor >> 16, (newColor >> 8) & 0xff, newColor & 0xff };

					for (uint c = 0; c < 3; ++c)
						soFar[c] += palcolor[c] * chosenAmount;
				}
			} else {
				// Use the gamma corrected planning algorithm if the palette has more than 2 colors
				Dictionary<uint, uint> solution = new Dictionary<uint, uint>();

				double currentPenalty = -1;

				uint chosenIndex = 0;
				for (uint i = 0; i < palette.Length; ++i) {
					byte r = (byte)((palette[i] >> 16) & 0xff);
					byte g = (byte)((palette[i] >> 8) & 0xff);
					byte b = (byte)(palette[i] & 0xff);

					double penalty = ColorCompare(inputRgb[0], inputRgb[1], inputRgb[2], r, g, b);
					if (penalty < currentPenalty || currentPenalty < 0) {
						currentPenalty = penalty;
						chosenIndex = i;
					}
				}
				solution[chosenIndex] = colorCount;

				double dblLimit = 1.0 / colorCount;
				while (currentPenalty != 0.0) {
					double bestPenalty = currentPenalty;
					uint bestSplitFrom = 0;
					uint[] bestSplitTo = new uint[] { 0, 0 };

					foreach (KeyValuePair<uint, uint> i in solution) {
						uint splitColor = i.Key;
						uint splitCount = i.Value;

						double[] sum = new double[] { 0, 0, 0 };
						foreach (KeyValuePair<uint, uint> j in solution) {
							if (j.Key == splitColor) continue;
							sum[0] += gammaCorrect[j.Key, 0] * j.Value * dblLimit;
							sum[1] += gammaCorrect[j.Key, 1] * j.Value * dblLimit;
							sum[2] += gammaCorrect[j.Key, 2] * j.Value * dblLimit;
						}

						double portion1 = (splitCount / 2) * dblLimit;
						double portion2 = (splitCount - splitCount / 2) * dblLimit;
						for (uint a = 0; a < palette.Length; ++a) {
							uint firstb = 0;
							if (portion1 == portion2)
								firstb = a + 1;

							for (uint b = firstb; b < palette.Length; ++b) {
								if (a == b) continue;
								int lumadiff = (int)(luminance[a]) - (int)(luminance[b]);
								if (lumadiff < 0)
									lumadiff = -lumadiff;
								if (lumadiff > 80000) continue;

								double[] test = new double[] { 
								GammaUncorrect(sum[0] + gammaCorrect[a, 0] * portion1 + gammaCorrect[b, 0] * portion2),
								GammaUncorrect(sum[1] + gammaCorrect[a, 1] * portion1 + gammaCorrect[b, 1] * portion2),
								GammaUncorrect(sum[2] + gammaCorrect[a, 2] * portion1 + gammaCorrect[b, 2] * portion2) 
							};

								double penalty = ColorCompare(inputRgb[0], inputRgb[1], inputRgb[2],
									(byte)(test[0] * 255), (byte)(test[1] * 255), (byte)(test[2] * 255));

								if (penalty < bestPenalty) {
									bestPenalty = penalty;
									bestSplitFrom = splitColor;
									bestSplitTo[0] = a;
									bestSplitTo[1] = b;
								}

								if (portion2 == 0) break;
							}
						}
					}

					if (bestPenalty == currentPenalty) break;

					uint splitC = solution[bestSplitFrom];
					uint split1 = splitC / 2;
					uint split2 = splitC - split1;
					solution.Remove(bestSplitFrom);
					if (split1 > 0)
						solution[bestSplitTo[0]] = (solution.ContainsKey(bestSplitTo[0]) ? solution[bestSplitTo[0]] : 0) + split1;
					if (split2 > 0)
						solution[bestSplitTo[1]] = (solution.ContainsKey(bestSplitTo[1]) ? solution[bestSplitTo[1]] : 0) + split2;
					currentPenalty = bestPenalty;
				}

				uint n = 0;
				foreach (KeyValuePair<uint, uint> i in solution)
					for (uint c = 0; c < i.Value; c++)
						mixingPlan[n++] = i.Key;
			}

			// Sort the colors by luminance and return the mixing plan
			Array.Sort(mixingPlan, delegate(uint index1, uint index2) {
				return luminance[index1].CompareTo(luminance[index2]);
			});
			return mixingPlan;
		}

	}

}