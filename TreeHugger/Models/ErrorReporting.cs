using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeHugger.Models
{
    public class ErrorReporting
    {
        public enum TreeAdditionError
        {
            InvalidIdLength,
            InvalidTreeName,
            TreeAlreadyExists,
            EmptyLocation,
            NoLatitudePresent,
            NoLongitudePresent,
            InvalidLatitudeValue,
            InvaliLongtitudeValue,
            DBAdditionError,
            DuplicateTreeError,
            NoTreeImagePresent,
            NoError
        }
        public enum EditTreeError
        {
            InvalidIdLength,
            InvalidTreeName,
            TreeAlreadyExists,
            EmptyLocation,
            NoLatitudePresent,
            NoLongitudePresent,
            InvalidLatitudeValue,
            InvalidLongitudeValue,
            DBEditError,
            DuplicateTreeError,
            NoTreeImagePresent,
            NoError
        }

        public enum SpeciesAdditionError
        {  
            InvalidIdLength, 
            InvalidSpeciesName,
            DuplicateSpeciesError,
            DBAdditionError,
            NoExampleImageError,
            NoError

        }
        public enum EditSpeciesError 
        {
            InvalidIdLength,
            InvalidTreeName,
            TreeAlreadyExists,
            EmptyLocation,
            NoLatitudePresent,
            NoLongitudePresent,
            InvalidLatitudeValue,
            InvaliLongtitudeValue,
            DBEditError,
            DuplicateTreeError,
            NoTreeImagePresent,
            NoError
        }
    }
}
