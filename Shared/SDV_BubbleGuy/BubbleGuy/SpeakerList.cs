using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewWeb.Speaker
{
    
    class SpeakerList : IEnumerable<SpeakerItem>
    {
        public class ListItem : SpeakerItem, IEnumerator<SpeakerItem>
        {
            public void AddItem(SpeakerItem oNew)
            {

            }
            public new SpeakerItem Current
            {
                get;
                private set;
            }
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
        public IEnumerator<SpeakerItem> GetEnumerator()
        {
            return new ListItem();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }
    }
}
