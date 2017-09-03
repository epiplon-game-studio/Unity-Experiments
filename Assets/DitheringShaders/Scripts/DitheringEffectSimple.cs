using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Dithering Simple")]
public class DitheringEffectSimple : ImageEffectBase {
		public int ColorCount = 4;
		public int PaletteHeight = 64;
		public Texture PaletteTexture;

		void OnRenderImage(RenderTexture source, RenderTexture destination) {
				material.SetFloat("_ColorCount", ColorCount);
				material.SetFloat("_PaletteHeight", PaletteHeight);
				material.SetTexture("_PaletteTex", PaletteTexture);
				Graphics.Blit(source, destination, material);
		}
}
