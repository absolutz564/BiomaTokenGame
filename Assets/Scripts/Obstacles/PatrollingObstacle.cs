using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PatrollingObstacle : Obstacle
{
    static int s_SpeedRatioHash = Animator.StringToHash("SpeedRatio");
    static int s_DeadHash = Animator.StringToHash("Dead");

    [Tooltip("Minimum time to cross all lanes.")]
    public float minTime = 2f;
    [Tooltip("Maximum time to cross all lanes.")]
    public float maxTime = 5f;
    [Tooltip("Leave empty if no animation")]
    public Animator animator;
    public Animator animator2;
    public Animator animator3;

    public AudioClip[] patrollingSound;

    protected TrackSegment m_Segement;

    protected Vector3 m_OriginalPosition = Vector3.zero;
    protected float m_MaxSpeed;
    protected float m_CurrentPos;

    protected AudioSource m_Audio;
    public bool m_isMoving = false;

    protected float k_LaneOffsetToFullWidth = 2f;

    public override IEnumerator Spawn(TrackSegment segment, float t)
    {
        Vector3 position;
        Quaternion rotation;
        segment.GetPointAt(t, out position, out rotation);
        AsyncOperationHandle op = Addressables.InstantiateAsync(gameObject.name, position, rotation);
        yield return op;
        if (op.Result == null || !(op.Result is GameObject))
        {
            Debug.LogWarning(string.Format("Unable to load obstacle {0}.", gameObject.name));
            yield break;
        }
        GameObject obj = op.Result as GameObject;

        obj.transform.SetParent(segment.objectRoot, true);

        PatrollingObstacle po = obj.GetComponent<PatrollingObstacle>();
        po.m_Segement = segment;

        //TODO : remove that hack related to #issue7
        Vector3 oldPos = obj.transform.position;
        obj.transform.position += Vector3.back;
        obj.transform.position = oldPos;

        po.Setup();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "UnlockPatrol" && !GameController.Instance.isTutorial)
        {
            if (this.tag == "ObstacleGuaxinim")
            {
                Transform Rastros = this.transform.Find("rastros");
                if (Rastros != null)
                {
                    Rastros.SetParent(this.transform.parent);

                    Rastros.gameObject.SetActive(true);
                    Rastros.localPosition = new Vector3(transform.localPosition.x - 21.5f, transform.localPosition.y, transform.localPosition.z);
                }
            }
            Debug.Log("Liberou o guaxinim");
            m_isMoving = true;
        }
    }
    public override void Setup()
    {

        float actualTime = Random.Range(minTime, maxTime);
        if (this.tag == "ObstacleGuaxinim")
        {
            Transform Rastros = this.transform.Find("rastros");
            if (Rastros != null)
            {
                if (GameController.Instance.isTutorial)
                {
                    Rastros.gameObject.SetActive(false);
                }
            }
            k_LaneOffsetToFullWidth = 150f;
            actualTime = Random.Range(minTime * 10, maxTime * 10);
            m_OriginalPosition = new Vector3(transform.localPosition.x + 20 + transform.right.x * m_Segement.manager.laneOffset, transform.localPosition.y + transform.right.y * m_Segement.manager.laneOffset, transform.localPosition.z + transform.right.z * m_Segement.manager.laneOffset);

        }
        else
        {
            m_OriginalPosition = transform.localPosition + transform.right * m_Segement.manager.laneOffset;
        }

        m_Audio = GetComponent<AudioSource>();
        if (m_Audio != null && patrollingSound != null && patrollingSound.Length > 0)
        {
            m_Audio.loop = true;
            m_Audio.clip = patrollingSound[Random.Range(0, patrollingSound.Length)];
            m_Audio.Play();
        }


        transform.localPosition = m_OriginalPosition;


        //time 2, becaus ethe animation is a back & forth, so we need the speed needed to do 4 lanes offset in the given time
        m_MaxSpeed = (m_Segement.manager.laneOffset * k_LaneOffsetToFullWidth * 2) / actualTime;

        if (animator != null)
        {
            AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            animator.SetFloat(s_SpeedRatioHash, clip.length / actualTime);
        }
        if (animator2 != null)
        {
            AnimationClip clip = animator2.GetCurrentAnimatorClipInfo(0)[0].clip;
            animator2.SetFloat(s_SpeedRatioHash, clip.length / actualTime);
        }
        if (animator3 != null)
        {
            AnimationClip clip = animator3.GetCurrentAnimatorClipInfo(0)[0].clip;
            animator3.SetFloat(s_SpeedRatioHash, clip.length / actualTime);
        }

        if (this.tag == "ObstacleGuaxinim")
        {
            m_isMoving = false;
        }
        else
        {
            m_isMoving = true;
        }
    }

    public override void Impacted()
    {
        m_isMoving = false;
        base.Impacted();

        if (animator != null)
        {
            animator.SetTrigger(s_DeadHash);
        }
        if (animator2 != null)
        {
            animator2.SetTrigger(s_DeadHash);
        }
        if (animator3 != null)
        {
            animator3.SetTrigger(s_DeadHash);
        }
    }
    void Update()
    {
        if (!m_isMoving)
            return;

        m_CurrentPos += Time.deltaTime * m_MaxSpeed;


        transform.localPosition = m_OriginalPosition - transform.right * Mathf.PingPong(m_CurrentPos, m_Segement.manager.laneOffset * k_LaneOffsetToFullWidth);
    }
}
