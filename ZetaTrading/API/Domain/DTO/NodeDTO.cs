namespace ZetaTrading.API.Domain.DTO
{
    public class NodeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NodeDTO> Children { get; set; }
    }
}
