using System.Data.Entity;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class DropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<ImageBankContext>
    {
        protected override void Seed(ImageBankContext context)
        {
            base.Seed(context);

            var setting1 = new Setting { Key = "OriginalImageRoot", Value = "~/Images/Upload/Original" };
            context.Settings.Add(setting1);

            var setting2 = new Setting { Key = "MediumImageRoot", Value = "~/Images/Upload/640x427" };
            context.Settings.Add(setting2);

            context.SaveChanges();
        }
    }
}