using UnityEngine;

public interface IInteractableSceneObject
{
    bool Interact();
    Transform GetThisTransform();
}
