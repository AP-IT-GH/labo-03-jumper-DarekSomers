using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public float gravityMod = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.runInBackground = true;
        Physics.gravity *= gravityMod;
    }
}
