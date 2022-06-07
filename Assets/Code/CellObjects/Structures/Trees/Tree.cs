namespace Code.CellObjects.Structures.Trees {
    public abstract class Tree : CellObject {
        public abstract bool Expands();

        protected override int Level() {
            return 0;
        }
    }
}
