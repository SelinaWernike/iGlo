using UnityEngine;

public interface ISelecionChangeObserver
{
    void onChange(GameObject previous, GameObject selected);
}