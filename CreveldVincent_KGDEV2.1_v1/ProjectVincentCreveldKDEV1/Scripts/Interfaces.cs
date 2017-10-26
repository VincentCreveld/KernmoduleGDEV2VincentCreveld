using UnityEngine;
/*
public interface IPassable {
    void DisableCollision();
    void EnableCollision();
}*/

public interface ICanPass {
}

public interface IBubblable {
    void GetTrapped();
    void Release();
}

public delegate void PoolEvent(GameObject p);

public interface IPoolable {
    void InitialiseObj(Vector3 v3, Transform t);
    void Recycle();
    PoolEvent PoolEvent {
        get;
        set;
    }
}
