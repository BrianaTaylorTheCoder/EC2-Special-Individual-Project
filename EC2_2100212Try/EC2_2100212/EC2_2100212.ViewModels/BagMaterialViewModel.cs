namespace EC2_2100212.ViewModels
{
        public class BagMaterialViewModel : CreateBagMaterialViewModel
        {
            public int Id { get; set; }

        }

        public class CreateBagMaterialViewModel
        {

            public int BagId { get; set; }
            public int MaterialId { get; set; }

        }
}

