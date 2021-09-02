namespace Projects.Domain.Tasks {
    public record Rate {
        public Rate(float? value) {
            Value = value ?? Free.Value;
        }
        
        public float Value { get; }

        public static Rate Free = new(0);
    }
}