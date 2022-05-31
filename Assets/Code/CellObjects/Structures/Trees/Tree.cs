namespace Code.CellObjects.Structures.Trees {
    public abstract class Tree : CellObject {
        public abstract bool Expands();
        
        public override int Level() {
            return 0;
        }
    }
}
