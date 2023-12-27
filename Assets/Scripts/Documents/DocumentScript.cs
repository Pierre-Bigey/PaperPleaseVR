using Entrants;
using UnityEngine;

namespace Documents
{
    public abstract class DocumentScript : MonoBehaviour
    {
        public DocumentType docType { get; protected set; }
    }
}
