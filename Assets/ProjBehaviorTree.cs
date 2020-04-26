using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class ProjBehaviorTree : MonoBehaviour
{

    public Light lamp;

    public GameObject participant1;
    public GameObject participant2;
    public GameObject participant3;
    public GameObject participant4;

    public Transform lightSwitchStandPoint;
    public GameObject lightSwitch;
    public InteractionObject lightSwitchIK;
    public Transform TVTurnStandPoint;
    public GameObject TVSwitch;
    public InteractionObject TVSwitchIK;

    public FullBodyBipedEffector hand;

    public Transform TVStandPoint;
    public Transform TVLookAtPoint;

    public Transform TVp1;
    public Transform TVp2;
    public Transform TVp3;

    public GameObject canvasLight;
    public GameObject canvasTV;
    public Text bubbleTextL;
    public Text bubbleTextT;

    public Transform p1;
    public Transform p2;
    public Transform p3;

    public GameObject ball;


    BehaviorMecanim part1;
    BehaviorMecanim part2;
    BehaviorMecanim part3;



    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start ()
    {

        part1 = participant1.GetComponent<BehaviorMecanim> ();
        part2 = participant2.GetComponent<BehaviorMecanim> ();
        part3 = participant3.GetComponent<BehaviorMecanim> ();

        canvasLight.GetComponent<CanvasGroup>().alpha = 0;
        canvasTV.GetComponent<CanvasGroup>().alpha = 0;
        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
        BehaviorManager.Instance.Register (behaviorAgent);
        behaviorAgent.StartBehavior ();


    }

    // Update is called once per frame
    void Update ()
    {

    }
    /*
       protected Node ST_ApproachAndWait(Transform target)
       {
       Val<Vector3> position = Val.V (() => target.position);
       return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
       }
       */

    protected Node faceAndPoint(BehaviorMecanim part, GameObject facing, int time)
    {
        Val<Vector3> face = Val.V (() => facing.transform.position);
        return new Sequence
            (
             part.Node_OrientTowards(face),
             part.Node_HandAnimation("pointing", true),
             new LeafWait(time),
             part.Node_HandAnimation("pointing", false)
            );
    }


    protected Node LightOff(BehaviorMecanim part)
    {
        Val<Vector3> position = Val.V (() => lightSwitchStandPoint.position);
        Val<Vector3> face = Val.V (() => lightSwitch.transform.position);
        return new Sequence
            (
             part.Node_GoTo(position),
             part.Node_OrientTowards(face),
             // part.Node_HandAnimation("pointing", true),
             part.Node_StartInteraction(hand, lightSwitchIK),
             new LeafInvoke(() => lamp.enabled = false),
             new LeafWait(500),
             // part.Node_HandAnimation("pointing", false)
             part.Node_StopInteraction(hand)
            );
    }

    protected Node TVOnOff(BehaviorMecanim part)
    {
        Val<Vector3> position = Val.V (() => TVTurnStandPoint.position);
        Val<Vector3> face = Val.V (() => TVSwitch.transform.position);
        return new Sequence
            (
             part.Node_GoTo(position),
             part.Node_OrientTowards(face),
             // part.Node_HandAnimation("pointing", true),
             part.Node_StartInteraction(hand, TVSwitchIK),
             new LeafWait(500),
             // part.Node_HandAnimation("pointing", false)
             part.Node_StopInteraction(hand)
            );
    }

    protected Node TextOn(String speech, GameObject canvas, Text t)
    {
        t.text = speech;
        return new Sequence
            (
             new LeafInvoke(() => canvas.GetComponent<CanvasGroup>().alpha = 1),
             new LeafWait(2000),
             new LeafInvoke(() => canvas.GetComponent<CanvasGroup>().alpha = 0)
            );

    }

    protected Node WatchTV(BehaviorMecanim part, Transform p)
    {
        Val<Vector3> tvpos = Val.V (() => TVLookAtPoint.position);
        Val<Vector3> standpos = Val.V (() => p.position);
        //Val<float> dist = Val.V (() => 2.0f);
        return new Sequence(part.Node_GoTo (standpos), part.Node_OrientTowards (tvpos));
    }

    private void updatePos(Val<Vector3> v, GameObject part)
    {
        part.GetComponent<SteeringController> ().Target = v.Value;
    }

    protected Node AssignRoles(GameObject parta, GameObject partb, GameObject partc)
    {
        //Val<bool> pp = Val.V (() => lamp.enabled);
        Func<bool> act = () => (lamp.enabled);

        Node trigger = new DecoratorLoop (new LeafAssert (act));

        return new Sequence (
                new SequenceParallel (this.faceAndPoint (parta.GetComponent<BehaviorMecanim>(), partb, 2000), this.TextOn ("You turn off the light", canvasLight, bubbleTextL)),
                new SequenceParallel (this.faceAndPoint (parta.GetComponent<BehaviorMecanim>(), partc, 2000), this.TextOn ("You turn on the TV", canvasTV, bubbleTextT)),
                new SequenceParallel (
                    this.WatchTV(parta.GetComponent<BehaviorMecanim>(), TVp1),
                    new Sequence(this.LightOff(partb.GetComponent<BehaviorMecanim>()), this.WatchTV(partb.GetComponent<BehaviorMecanim>(), TVp3)),
                    new Sequence(this.TVOnOff(partc.GetComponent<BehaviorMecanim>()), this.WatchTV(partc.GetComponent<BehaviorMecanim>(), TVp2))
                    ),

                new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success,
                        new SequenceParallel(
                            trigger,
                            new Sequence(
                                this.LightOff(partb.GetComponent<BehaviorMecanim>()),
                                this.WatchTV(partb.GetComponent<BehaviorMecanim>(), TVp3)))))
                );
    }
    protected Node pointOthers(GameObject parta, GameObject partb, GameObject partc)
    {
        return new SelectorShuffle (
                this.AssignRoles(parta, partb, partc),
                this.AssignRoles(parta, partc, partb),
                this.AssignRoles(partb, partc, parta),
                this.AssignRoles(partb, parta, partc),
                this.AssignRoles(partc, parta, partb),
                this.AssignRoles(partc, partb, parta)
                );
    }

    /*protected Node ST_GoToUpToRadius(BehaviorMecanim part, Transform wanderPoint, float distance, float speed)
      {
      Val<Vector3> wanderPosition = Val.V(() => wanderPoint.position);
    //distance has to be adjusted according to the speed
    Val<float> dist = Val.V(() => distance + Mathf.Clamp(speed, 1f, 6f) / 3f);
    Val<float> wanderSpeed = Val.V(() => speed);

    return part.Node_GoToUpToRadius(wanderPosition, dist, wanderSpeed);
    }*/

    /*protected Node ST_Grab(BehaviorMecanim part, Transform item)
      {
      Val<Vector3> itemPosition = Val.V(() => item.position);

      return part.Node_Grab(itemPosition);
      }*/

    protected Node BuildTreeRoot()
    {

        Val<Vector3> pos1 = Val.V (() => p1.position);
        Val<Vector3> pos2 = Val.V (() => p2.position);
        Val<Vector3> pos3 = Val.V (() => p3.position);

        //Val<Vector3> face = Val.V (() => participant3.transform.position);
        Node setup = new Sequence
            (
             /*
                new LeafInvoke(() => updatePos(pos1, participant1)),
                new LeafInvoke(() => updatePos(pos2, participant2)),
                new LeafInvoke(() => updatePos(pos3, participant3)),*/
             new SequenceParallel (
                 part1.Node_GoTo(pos1),
                 part2.Node_GoTo(pos2),
                 part3.Node_GoTo(pos3)),

             /* i would love to know why this freezes them afterwards
                this.ST_GoToUpToRadius(part1, p1, 2.0f, 4.0f),
                this.ST_GoToUpToRadius(part2, p1, 2.0f, 4.0f),
                this.ST_GoToUpToRadius(part3, p1, 2.0f, 4.0f)),*/


                 new LeafWait(500),

                 this.pointOthers(participant1, participant2, participant3)

                     );




        Node root = setup;


        Val<Vector3> face1 = Val.V(() => participant4.transform.position);
        Val<Vector3> face2 = Val.V(() => participant1.transform.position);

        //Node root = new Sequence(part1.Node_OrientTowards(face1), participant4.GetComponent<BehaviorMecanim>().Node_OrientTowards(face2));
        return root;
    }
}
