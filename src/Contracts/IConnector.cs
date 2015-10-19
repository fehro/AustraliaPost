using System.Collections.Generic;
using AustraliaPost.Models;

namespace AustraliaPost.Contracts
{
    public interface IConnector
    {
        IEnumerable<Locality> Search(string query, string postcode = null);
    }
}
