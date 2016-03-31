namespace GuessTheSong.Infrasctucture
{
    interface ICloneable<out T>
    {
        T Clone();
    }
}
