using System.Data.Entity;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class DropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<ImageBankContext>
    {
        protected override void Seed(ImageBankContext context)
        {
            base.Seed(context);

            var setting = new Setting {Key = "ImagesRoot", Value = "~/Images/Upload"};
            context.Settings.Add(setting);

            context.SaveChanges();
        }
    }
}