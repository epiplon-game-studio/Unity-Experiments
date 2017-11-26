# Dungeon

Dungeon generator with different implementations.
* [BSP](#bsp) (binary space partition)
* (more coming later)

## Classes

| Acess Level |   Class Name   | Description
|-------------|----------------|-------------
| public sealed |  DungeonMaster | Main Unity Component. Can choose between dungeons in a list. Each dungeon algorithm uses an interface 
| public abstract | Dungeon | Base class for a dungeon implementation

### public sealed class DungeonMaster : MonoBehaviour

* `[SerializeField] int width` - __Total Width__
* `[SerializeField] int height`- __Total Height__
* `public int Width` - __GET__
* `public int Height`- __GET__ 
* `[SerializeField] Dungeon selectedDungeon` - __Dungeon selected by the user in Unity interface__ 

+ `public void Create()`

### public abstract class Dungeon : MonoBehaviour

* `public UnityEvent OnGenerateStart`
* `public UnityEvent OnGenerateFinish`

+ `public abstract void Generate(DungeonMaster dungeonMaster)`

## Dungeons Algorithms

### <a name="bsp">BSP</a>

#### public class BSPDungeon : Dungeon

* `BSPLeaf root`
* `int minHorizontal`
* `int minVertical`

#### public class BSPLeaf

* `public Rect room`
* `public BSPLeaf right`
* `public BSPLeaf left`

#### public enum Divide 

* `Horizontal`
* `Vertical`
