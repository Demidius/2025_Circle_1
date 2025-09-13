namespace Code.TODO
{
    public interface ITankInputProvider
    {
        float LeftTrackInput  { get; } 
        float RightTrackInput { get; } 
        bool ToggleEnginePressed { get; }  
    }

}
