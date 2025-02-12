extends Node2D

onready var fireflies = $Fireflies
onready var butterflies = $Butterflies
onready var water_reflections = $WaterReflections
onready var leaf_particles = $LeafParticles
var time = 0
var wind_influence = Vector2.ZERO

func _ready():
	# Start with default states
# 	fireflies.emitting = false
# 	butterflies.emitting = true
# 	water_reflections.emitting = true
# 	leaf_particles.emitting = false
	
	# Connect to wind controller
	var wind_controller = get_node("/root/Main/WindController")
	if wind_controller:
		wind_controller.connect("wind_changed", self, "_on_wind_changed")

func _process(_delta):
	time += _delta
	var hud = get_node("/root/Main/HUD")
	var day_night = get_node("/root/Main/DayNightCycle")
	
	if hud && day_night:
		var time_cycle = day_night.time
		var is_night = time_cycle > 0.75 || time_cycle < 0.25
# 		var weather = hud.current_weather
		
		# Handle fireflies with pulsing glow and wind influence
# 		fireflies.emitting = is_night && (weather == "Sunny" || weather == "Cloudy")
# 		if fireflies.emitting:
# 			var pulse = (sin(time * 2) + 1) * 0.5
# 			fireflies.scale_amount = 2 + pulse
# 			fireflies.color.a = 0.7 + pulse * 0.3
# 			fireflies.direction = wind_influence
		
		# Make butterflies react to wind
# 		butterflies.emitting = !is_night && (weather == "Sunny" || weather == "Cloudy")
# 		if butterflies.emitting:
# 			butterflies.direction = wind_influence
# 			butterflies.initial_velocity = 30 + wind_influence.length() * 20
		
		# Enhance leaf particles with wind
# 		leaf_particles.emitting = weather == "Stormy" || wind_influence.length() > 0.3
# 		if leaf_particles.emitting:
# 			leaf_particles.direction = wind_influence
# 			leaf_particles.initial_velocity = 40 + wind_influence.length() * 50
		
		# Make water reflections respond to wind
# 		water_reflections.amount = 50 if weather == "Sunny" else 30
# 		water_reflections.initial_velocity = 5 + wind_influence.length() * 10
# 		water_reflections.direction = wind_influence

func _on_wind_changed(strength, direction):
	wind_influence = direction * strength
