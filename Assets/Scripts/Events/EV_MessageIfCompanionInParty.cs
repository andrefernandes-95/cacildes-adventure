using System.Collections;
using System.Linq;
using AF.Dialogue;
using AF.Companions;

namespace AF
{
    public class EV_MessageIfCompanionInParty : EV_SimpleMessage
    {
        public CompanionsDatabase companionDatabase;
        public CompanionID companionID;

        public override IEnumerator Dispatch()
        {
            if (companionDatabase.IsCompanionAndIsActivelyInParty(companionID.GetCompanionID()))
            {
                yield return base.Dispatch();
            }
            else
            {
                yield return null;
            }
        }

    }
}
