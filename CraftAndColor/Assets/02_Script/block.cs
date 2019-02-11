using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {

    PointEffector2D pe;
    public float attraction = -10; //引力 Attraction
    public float repulsion = 10; //斥力  Repulsive force
    public bool TTB = false;//ブロックに触れているかどうか
    //public Collider2D col = null;

    void Start () {
        //This time we are using "PointEffector2D" component.
        this.pe = this.gameObject.GetComponentInChildren<PointEffector2D>(); 
    }

    /*
    private void Update()
    {
        if (TTB)
        {
            //MagneticManipulation();
            pe.forceMagnitude = attraction;
            Debug.Log(this.name + "Attraction");
        }
        if(!TTB)
        {
            pe.forceMagnitude = 0;
            Debug.Log(this.name + "No power is working");
            Debug.Log("あんたばか"+ pe.forceMagnitude);
        }
        
    }*/


    public void liftBlock(Transform lifterTransform)
    {        
        transform.parent = lifterTransform;
    }

    public void downBlock(Transform lifterTransform)
    {
        Vector3 PosBox = transform.localPosition;
        transform.localPosition = PosBox;

        transform.parent = null;

    }

    public　void MagneticManipulation(Collider2D col)
    {
        if (col.tag == this.tag)
        {
            pe.forceMagnitude = attraction;
            Debug.Log(this.name + "Attraction");
        }
        else if (col.tag != this.tag)
        {
            pe.forceMagnitude = repulsion;
            Debug.Log(this.name + "Repulsion");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == this.tag)
        {
            pe.forceMagnitude = attraction;
            Debug.Log(this.name + "Attraction");
        }
        else if (collision.tag != this.tag)
        {
            pe.forceMagnitude = repulsion;
            Debug.Log(this.name + "Repulsion");
        }
        //MagneticManipulation(collision);

        //if (collision.tag == this.tag) TTB = true;
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == this.tag)
        {
            pe.forceMagnitude = attraction;
            Debug.Log(this.name + "Attraction");
        }
        else if (collision.tag != this.tag)
        {
            pe.forceMagnitude = repulsion;
            Debug.Log(this.name + "Repulsion");
        }

        else
        {
            pe.forceMagnitude = 0;
            Debug.Log(this.name + "No power is working");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.tag == this.tag) TTB = false;
        pe.forceMagnitude = 0;
        Debug.Log(this.name + "No power is working");
    }
}
