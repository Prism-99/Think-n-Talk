

using Microsoft.Xna.Framework.Graphics;
using SDV_Speaker.Speaker;

namespace SDV_BubbleGuy.SMAPIInt
{
    public class BubbleGuyAPI : IModApi
    {
        private BubbleGuyManager oManager;
        public BubbleGuyAPI(BubbleGuyManager manager)
        {
            oManager = manager;
        }
        public void ClearBubble()
        {
            oManager.RemoveBubbleGuy(false, false);
        }

        public Texture2D GetTalkBubble()
        {
            return oManager.GetTalkBubble();
        }

        public Texture2D GetThinkBubble()
        {
            return oManager.GetThinkBubble();
        }

        public void SetTalkBubble(Texture2D talkbubble)
        {
            oManager.SetTalkBubble(talkbubble);
        }

        public void SetThinkBubble(Texture2D thinkbubble)
        {
            oManager.SetThinkBubble(thinkbubble);
        }

        public void ShowTalkBubble(string text)
        {
            oManager.AddBubbleGuy(false, text);
        }

        public void ShowThinkBubble(string text)
        {
            oManager.AddBubbleGuy(true, text);
        }
    }
}
