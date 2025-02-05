using UnityEngine;

public interface IService
{

}
public interface IEndGame{
    public void Init(ParticleSystem[] fireworks);
    public void OpenUI();
}
public interface ITimeEnd{
    public void SetTime();
}