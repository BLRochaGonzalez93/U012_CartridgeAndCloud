namespace VRMGames.CartridgeAndCloud.Application.Audio
{
    public interface IAudioRouter
    {
        void Play(
            string eventId);

        void SetChannelVolume(
            AudioChannel channel,
            float normalizedVolume);

        float GetChannelVolume(
            AudioChannel channel);
    }

    public enum AudioChannel
    {
        Music = 0,
        Ambience = 1,
        Ui = 2,
        Effects = 3
    }
}
