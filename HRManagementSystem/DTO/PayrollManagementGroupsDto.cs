namespace HRManagementSystem.DTO
{
    public class PayrollManagementGroupsDto
    {
        public DateTime Month { get; set; }
        public List<PayrollManagementDto> Payrolls { get; set; }
    }
}
