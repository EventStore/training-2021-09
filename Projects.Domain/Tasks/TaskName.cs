namespace Projects.Domain.Tasks {
    public record TaskName {
        public TaskName(string value) => Value = string.IsNullOrWhiteSpace(value) ? "Unnamed task" : value;

        public string Value { get; }
    }
}