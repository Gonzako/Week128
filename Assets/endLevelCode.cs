/*
 *
 * Copyright (c) Gonzako
 * Gonzako123@gmail.com
 *
 */
 
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class endLevelCode : MonoBehaviour
{
    #region Public Fields
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        SceneManager.LoadScene("endLevel");

    }

    #endregion
#endif

}