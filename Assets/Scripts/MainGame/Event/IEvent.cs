using Cysharp.Threading.Tasks;

public interface IEvent
{
    UniTask ExecuteEvent(EventContext context, int param);
}
