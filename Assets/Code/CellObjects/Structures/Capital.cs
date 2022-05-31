using Code.Control.Exceptions;

public class Capital : CellObject {
    private int _playerId;

    public int PlayerId => _playerId;

    public void SetPlayerId(int id) {
        _playerId = id;
    }

    public override int GetPrice() {
        throw new ThatShouldntBeUsedException();
    }

    public override int GetMaintenanceCost() {
        throw new ThatShouldntBeUsedException();
    }

    public override int Level() {
        return 1;
    }
    
    public override int Range() {
        return 0;
    }
}
