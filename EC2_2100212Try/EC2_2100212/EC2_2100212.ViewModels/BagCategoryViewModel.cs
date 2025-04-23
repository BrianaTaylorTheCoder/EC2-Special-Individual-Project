// Name: Briana Taylor
// ID: 2100212
// Lecturer: Mr. Philip Smith
// Module: Enterprise Computing 2
// Semester: 2
// Year: 2025

namespace EC2_2100212.ViewModels
{
    public class BagCategoryViewModel : CreateBagCategoryViewModel
    {
        public int Id { get; set; }

    }

    public class CreateBagCategoryViewModel
    {

        public int BagId { get; set; }
        public int CategoryId { get; set; }

    }
}
