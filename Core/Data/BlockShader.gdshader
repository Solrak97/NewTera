shader_type spatial;

void fragment() {
    //vec3 dirt = vec3(0.55, 0.27, 0.07); // Base dirt color
    float light_intensity = dot(NORMAL, vec3(0.5, 0.5, 1.0)); // Fake lighting
    light_intensity = step(0.5, light_intensity); // Turns shading into two tones

    ALBEDO = COLOR.rgb; // * light_intensity;
}