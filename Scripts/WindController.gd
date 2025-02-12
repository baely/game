extends Node

var wind_strength = 0.0
var wind_direction = Vector2.RIGHT
var time = 0

signal wind_changed(strength, direction)

func _ready():
	randomize()

func _process(delta):
	time += delta
	var hud = get_node("/root/Main/HUD")
	
	if hud:
		var weather = hud.current_weather
		
		# Update wind based on weather
		match weather:
			"Sunny":
				wind_strength = lerp(wind_strength, 0.1, 0.1)
			"Cloudy":
				wind_strength = lerp(wind_strength, 0.3, 0.1)
			"Rainy":
				wind_strength = lerp(wind_strength, 0.5, 0.1)
			"Stormy":
				wind_strength = lerp(wind_strength, 1.0, 0.1)
				# Add wind gusts during storms
				wind_strength += sin(time * 3) * 0.2
		
		# Gradually change wind direction
		var target_direction = Vector2.RIGHT.rotated(sin(time * 0.2) * PI)
		wind_direction = wind_direction.linear_interpolate(target_direction, 0.05)
		
		# Emit signal for other nodes to respond to wind changes
		emit_signal("wind_changed", wind_strength, wind_direction)
