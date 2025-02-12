extends CanvasLayer

var time = 0
var weather_types = ["Sunny", "Cloudy", "Rainy", "Stormy"]
var current_weather = "Sunny"
var weather_timer = 0
var weather_duration = 120  # Weather changes every 2 minutes
var particles

onready var time_label = $TimeLabel
onready var stamina_bar = $StaminaBar

func _ready():
	particles = CPUParticles2D.new()
	particles.emitting = false
	particles.amount = 100
	particles.lifetime = 2.0
	particles.emission_shape = CPUParticles2D.EMISSION_SHAPE_BOX
	particles.emission_box_extents = Vector2(500, 1)
	particles.direction = Vector2(0, 1)
	particles.gravity = Vector2(0, 98)
	particles.initial_velocity = 50
	add_child(particles)
	particles.position = Vector2(get_viewport().size.x/2, -10)
	
	$MarginContainer/HBoxContainer/TimeLabel.text = "12:00"
	$MarginContainer/HBoxContainer/WeatherPanel/WeatherLabel.text = current_weather
	$MarginContainer/HBoxContainer/Stamina.value = 100
	$MarginContainer/HBoxContainer/Energy.value = 100
	
	# Start weather cycle
	_on_weather_timeout()

func _process(delta):
	time += delta
	
	# Update time label
	var hours = int(fmod(time / 25, 24))  # 1 real second = 1 game minute
	var minutes = int(fmod(time, 25) * 60 / 25)
	$MarginContainer/HBoxContainer/TimeLabel.text = "%02d:%02d" % [hours, minutes]
	
	# Update stamina from character
	var player = get_parent().get_node("Character")
	if player:
		stamina_bar.value = player.stamina
	
	# Handle weather changes
	weather_timer += delta
	if weather_timer >= weather_duration:
		weather_timer = 0
		_on_weather_timeout()
	
	# Handle particles based on weather
	match current_weather:
		"Rainy":
			if !particles.emitting:
				particles.modulate = Color(0.5, 0.5, 1.0, 0.5)
				particles.emitting = true
		"Stormy":
			if !particles.emitting:
				particles.modulate = Color(0.3, 0.3, 0.5, 0.7)
				particles.initial_velocity = 100
				particles.emitting = true
		_:
			particles.emitting = false
	
	# Update energy based on current time and weather
	var energy = 100
	var cycle = fmod(time / 25, 24)
	
	# Reduce energy at night
	if cycle < 6 || cycle > 20:  # Between 8 PM and 6 AM
		energy -= 30
	
	# Reduce energy based on weather
	match current_weather:
		"Cloudy":
			energy -= 10
		"Rainy":
			energy -= 20
		"Stormy":
			energy -= 30
	
	$MarginContainer/HBoxContainer/Energy.value = energy

func _on_weather_timeout():
	current_weather = weather_types[randi() % weather_types.size()]
	$MarginContainer/HBoxContainer/WeatherPanel/WeatherLabel.text = current_weather
	
	# Update energy based on weather
	match current_weather:
		"Sunny":
			$MarginContainer/HBoxContainer/Energy.value = 100
		"Cloudy":
			$MarginContainer/HBoxContainer/Energy.value = 75
		"Rainy":
			$MarginContainer/HBoxContainer/Energy.value = 50
		"Stormy":
			$MarginContainer/HBoxContainer/Energy.value = 25
