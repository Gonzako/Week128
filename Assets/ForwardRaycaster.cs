/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */
 
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ForwardRaycaster : MonoBehaviour
{
    #region Public Fields
    public LayerMask playerLayers;
    public float maxDistance = 2;
    #endregion
 
    #region Private Fields
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion


    #if true
    #region Unity API

    void Start()
    {

    }
 
    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, new Vector2(transform.localScale.x, 0) * maxDistance + (Vector2)transform.position, Color.red);
        var hit = Physics2D.Raycast(transform.position,new Vector2(transform.localScale.x, 0), maxDistance, playerLayers);
        if (hit)
        {
            if (hit.collider.GetComponent<stealthHandler>().Visible)
            {
                var whats = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(whats, LoadSceneMode.Single);
            }
        }
    }

    #endregion
    #endif
 
}