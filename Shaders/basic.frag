#version 330 core
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

void main()
{
    vec3 lightDir = normalize(vec3(1.0, 1.0, 1.0));
    vec3 normal = normalize(Normal);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * vec3(1.0);
    vec3 ambient = vec3(0.1);
    vec3 result = (ambient + diffuse) * vec3(0.7, 0.7, 0.7);
    FragColor = vec4(result, 1.0);
} 