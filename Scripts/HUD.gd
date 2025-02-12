extends CanvasLayer

@onready var time_label: Label = $MarginContainer/HBoxContainer/TimeLabel
@onready var stamina_bar: ProgressBar = $MarginContainer/HBoxContainer/Stamina

var time: float = 0.0
var weather_types: Array[String] = ["Sunny", "Cloudy", "Rainy", "Stormy"]
var current_weather: String = "Sunny"
var weather_timer: float = 0.0
var weather_duration: float = 120.0  # Weather changes every 2 minutes
var particles: GPUParticles2D

func _ready() -> void:
	particles = GPUParticles2D.new()
	particles.emitting = false
	particles.amount_ratio = 1
	particles.lifetime = 2.0
	
	var material = ParticleProcessMaterial.new()
	material.emission_shape = ParticleProcessMaterial.EmissionShape.BOX
	material.emission_box_extents = Vector3(500, 1, 0)
	material.direction = Vector3(0, 1, 0)
	material.gravity = Vector3(0, 98, 0)
	material.initial_velocity_min = 50.0
	material.initial_velocity_max = 50.0
	particles.process_material = material
	
	add_child(particles)
	particles.position = Vector2(get_viewport().get_visible_rect().size.x/2, -10)
	
	$MarginContainer/HBoxContainer/TimeLabel.text = "12:00"
	$MarginContainer/HBoxContainer/WeatherPanel/WeatherLabel.text = current_weather
	$MarginContainer/HBoxContainer/Stamina.value = 100.0
	$MarginContainer/HBoxContainer/Energy.value = 100.0
	
	# Start weather cycle
	_on_weather_timeout()

func _process(delta: float) -> void:
	time += delta
	
	# Update time label
	var hours: int = int(fmod(time / 25.0, 24))  # 1 real second = 1 game minute
	var minutes: int = int(fmod(time, 25.0) * 60.0 / 25.0)
	time_label.text = "%02d:%02d" % [hours, minutes]
	
	# Update stamina from character
	var player = get_parent().get_node("Character") as Node
	if player:
		stamina_bar.value = player.get("stamina")
	
	# Handle weather changes
	weather_timer += delta
	if weather_timer >= weather_duration:
		weather_timer = 0.0
		_on_weather_timeout()
	
	# Handle particles based on weather
	match current_weather:
		"Rainy":
			if !particles.emitting:
				particles.modulate = Color(0.5, 0.5, 1.0, 0.5)
				var material = particles.process_material as ParticleProcessMaterial
				if material:
					material.initial_velocity_min = 50.0
					material.initial_velocity_max = 50.0
				particles.emitting = true
		"Stormy":
			if !particles.emitting:
				particles.modulate = Color(0.3, 0.3, 0.5, 0.7)
				var material = particles.process_material as ParticleProcessMaterial
				if material:
					material.initial_velocity_min = 100.0
					material.initial_velocity_max = 100.0
				particles.emitting = true
		_:
			particles.emitting = false
	
	# Update energy based on current time and weather
	var energy: float = 100.0
	var cycle: float = fmod(time / 25.0, 24.0)
	
	# Reduce energy at night
	if cycle < 6.0 or cycle > 20.0:  # Between 8 PM and 6 AM
		energy -= 30.0
	
	# Reduce energy based on weather
	match current_weather:
		"Cloudy":
			energy -= 10.0
		"Rainy":
			energy -= 20.0
		"Stormy":
			energy -= 30.0
	
	$MarginContainer/HBoxContainer/Energy.value = energy

func _on_weather_timeout() -> void:
	current_weather = weather_types[randi() % weather_types.size()]
	$MarginContainer/HBoxContainer/WeatherPanel/WeatherLabel.text = current_weather
	
	# Update energy based on weather
	match current_weather:
		"Sunny":
			$MarginContainer/HBoxContainer/Energy.value = 100.0
		"Cloudy":
			$MarginContainer/HBoxContainer/Energy.value = 75.0
		"Rainy":
			$MarginContainer/HBoxContainer/Energy.value = 50.0
		"Stormy":
			$MarginContainer/HBoxContainer/Energy.value = 25.0
