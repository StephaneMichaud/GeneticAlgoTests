
using UnityEngine;
using System.Collections;
using System;
using Assets;
public class MouvementScript : MonoBehaviour {

    //Pour une insitance de cube
    public Rigidbody rb;
    public Vector3 Direction;
    public float Velocity;
    public Camera CameraCube;
    RaycastHit SensorsRay;
    Ray FLeftSensor;
    Ray FRightSensor;
    Ray FSensor;
    double[] inputsSensor;
    //Pour levolutuion
    NeuralNetwork brain { get; set; }
    NetworkPopulation pop { get; set; }
    public float FitnessScore { get; set; }
    int index = 0;
    Vector3 posInitial;
    Vector3 dirIninitial;
    bool speed;
    Quaternion transInitial { get; set; }
    // Use this for initialization
    void Start ()
    {
        transInitial = transform.rotation;
        brain = new NeuralNetwork(3, new int[] { 2,3 });
        brain.SetActivationFunction(new ReLuActivation());
        pop = new NetworkPopulation(10, brain, 0.5f);
        pop.CreateFirstGeneration();
        inputsSensor = new double[3];
        posInitial = transform.position;
        dirIninitial = Direction;
        Time.timeScale = 5;
        brain = pop.Generations[0][0].ReturnNetwork(brain);
    }
	
	//is called once per frame
	void FixedUpdate ()
    {
        transform.Translate(2*Direction * Time.deltaTime);
        CapterDistances();
        double[] outputs = brain.Compute(inputsSensor);
        /* if (outputs[0] > 0.33)
         {
             if (outputs[0] > 0.66)
                 Rotate(1);
             else
                 Rotate(-1);
         }*/
        // Rotate((float)outputs[0]);
        int cMax = 0;
        double valueMax = outputs[0];
        for (int i = 1; i < outputs.Length; i++)
        {
            if(outputs[i]>valueMax)
            {
                valueMax = outputs[i];
                cMax = i;
            }
        }

        switch (cMax)
        {
           case 0:
                Rotate(-1);
                break;
           case 1:
                Rotate(1);
                break;
        }

        // Rotate((float)outputs[0]-0.5f);
        FitnessScore+=0.1f;
        UpdateCamera();
        //transform.

    }
    /// <summary>
    /// Modifie la direction selon un angle spcifique vers la gauche ou vers la droite
    /// </summary>
    /// <param name="right"></param>
    void Rotate(float dir)
    {

      //  if(dir)
       Direction = Matrix4x4.Rotate(Quaternion.Euler(0, 100 * Time.deltaTime*(dir), 0)) * Direction;
       Direction= Direction.normalized;

      //  Direction = new Vector3(Direction.x, Direction.y, 0);
        //else
          //  Direction = Matrix4x4.Rotate(Quaternion.Euler(0, 100 * Time.deltaTime * (-0.5f), 0)) * Direction;
    }
    /// <summary>
    /// Transforme la position et lorientation de la camera selon la direction et la position du cube
    /// </summary>
    void UpdateCamera()
    {
        CameraCube.transform.position = transform.position;
        CameraCube.transform.rotation = Quaternion.LookRotation(new Vector3(Direction.normalized.x, 0, Direction.normalized.z));
        CameraCube.transform.position += -5 * Direction + new Vector3(0, 2, 0);
    }

    void CapterDistances()
    {
        //Selon la regle de la main droite devrait faire des vectors de chaque cote du cube (inverser?)
        FLeftSensor = new Ray(transform.position, Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0))*Vector3.Cross(Vector3.down, Direction));
        FRightSensor = new Ray(transform.position, Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0)) * Vector3.Cross(Vector3.up, Direction));
        FSensor = new Ray(transform.position, Direction);
        RaycastHit Hit;
        //Permet de calculer la distance entre cube et mur peut importe leur orientation
        inputsSensor = new double[] { -1,-1,-1 };
        if (Physics.Raycast(FLeftSensor, out Hit,2))
        {
            Debug.DrawRay(transform.position, FLeftSensor.direction.normalized * (transform.position - Hit.point).magnitude, Color.red, Time.deltaTime, true);
            inputsSensor[0] = 1;
        }
        else
            Debug.DrawRay(transform.position, FLeftSensor.direction.normalized * 2, Color.green, Time.deltaTime, true);

        if (Physics.Raycast(FRightSensor, out Hit,2))
        {
            Debug.DrawRay(transform.position, FRightSensor.direction.normalized * (transform.position - Hit.point).magnitude, Color.red, Time.deltaTime, true);
            inputsSensor[1] = 1;
        }
        else
            Debug.DrawRay(transform.position, FRightSensor.direction.normalized * 2, Color.green, Time.deltaTime, true);

        if (Physics.Raycast(FSensor, out Hit,3))
        {
            Debug.DrawRay(transform.position, FSensor.direction.normalized * (transform.position - Hit.point).magnitude, Color.red, Time.deltaTime, true);
            inputsSensor[2] = 1;
        }
        else
            Debug.DrawRay(transform.position, FSensor.direction.normalized * 3, Color.green, Time.deltaTime, true);
        //affichage des lignes des sensors

        Debug.ClearDeveloperConsole();
    }

    void OnCollisionEnter()
    {
        pop.Generations[pop.NbGeneration - 1][index].Fitness = (float)Math.Pow( FitnessScore,2);
        Debug.Log(pop.NbGeneration+": "+index+" ["+FitnessScore+"]");
        FitnessScore = 0;
        index++;
        if(index==pop.GenerationSize)
        {
            index = 0;
            pop.CreateNewGeneration();
        }
        transform.position = posInitial;
        Direction = dirIninitial;
        brain = pop.Generations[pop.NbGeneration - 1][index].ReturnNetwork(brain);
        transform.rotation = transInitial;
    }
        
}
