using Omni.Threading.Tasks;


public class LineThunder : SkillSingleServer
{
    protected override async void SkillSingleDemage()
    {
        for (int i = 0; i < 4; i++)
        {
            base.SkillSingleDemage();
            await UniTask.WaitForSeconds(0.6f);
        }

    }
}
