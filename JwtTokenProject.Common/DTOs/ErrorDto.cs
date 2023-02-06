namespace JwtTokenProject.Common.DTOs
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; } = new List<string>();
        public bool isShow { get; private set; }

        public ErrorDto(string erros, bool isShow)
        {
            Errors.Add(erros);
            isShow = true;
        }

        public ErrorDto(List<string> erros, bool isShow)
        {
            Errors = erros;
            isShow = true;
        }
    }
}
