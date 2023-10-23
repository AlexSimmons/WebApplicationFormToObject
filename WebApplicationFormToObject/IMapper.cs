using System.Web.UI.HtmlControls;

namespace WebApplicationFormToObject
{
    public interface IMapper
    {
        void LoadObjectToForm(HtmlForm form, object objectToLoad, bool lockFields = false, bool ignoreStrings = false);
        bool SaveFormToObject(HtmlForm form, object objectToSave, bool allowNullableParameter = false);
    }
}