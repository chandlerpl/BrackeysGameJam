using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AK.Wwise.Event _footstepSound;
    public AK.Wwise.Event _footstepRunSound;
    public AK.Wwise.Event _footstepCrouchSound;
    public AK.Wwise.Event _footstepBreathingSound;

    public float runDuration = 5f;

    private bool isRunning = false;
    private float previousBreath;
    public void Step()
    {
        _footstepSound.Post(gameObject);
        GameManager.Instance.AudioManager.PlaySound(new AudioData()
        {
            position = transform.position,
            range = 20,
            time = Time.time + 10f,
            priority = 1
        });

        if(isRunning)
        {
            isRunning = false;

/*            if (startTime + runDuration < Time.time)
            {
                
            }*/
        }
    }
    public void StepRun()
    {
        _footstepRunSound.Post(gameObject);
        GameManager.Instance.AudioManager.PlaySound(new AudioData()
        {
            position = transform.position,
            range = 20,
            time = Time.time + 10f,
            priority = 2
        });

        if (!isRunning)
        {
            isRunning = true;
            previousBreath = Time.time + runDuration;
        }

        if(previousBreath < Time.time)
        {
            _footstepBreathingSound.Post(gameObject);
            previousBreath += 1f;
        }
    }

    public void StepCrouch()
    {
        _footstepCrouchSound.Post(gameObject);
        GameManager.Instance.AudioManager.PlaySound(new AudioData()
        {
            position = transform.position,
            range = 10,
            time = Time.time + 5f,
            priority = 0
        });
    }
}
