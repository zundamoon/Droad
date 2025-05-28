using Cysharp.Threading.Tasks;

public interface ICondition
{
    UniTask<bool> IsCompleteCondition(EventContext context, int param);
}
