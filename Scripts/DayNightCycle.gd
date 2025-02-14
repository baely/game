extends Node

export var day_duration = 720.0  # 12 minutes for a full day
export var starting_hour = 8.0   # Start at 8 AM
export(Color) var day_color = Color(1.0, 1.0, 1.0, 1.0)
export(Color) var night_color = Color(0.07, 0.07, 0.15, 1.0)
export(Color) var dawn_dusk_color = Color(0.86, 0.70, 0.70, 1.0)

var time = 0.0
var current_hour = 0.0
var weather_effects = ["clear", "cloudy", "rain"]
var current_weather = "clear"
var weather_transition_time = 0.0
var weather_duration = 300.0  # 5 minutes per weather state

onready var canvas_modulate = $CanvasModulate

func _ready():
	time = (starting_hour / 24.0) * day_duration
	randomize()
	_change_weather()

func _process(delta):
	time += delta
	if time >= day_duration:
		time = 0.0
	
	current_hour = (time / day_duration) * 24.0
	_update_lighting()
	_update_weather(delta)

func _update_lighting():
	var color
	# Dawn (5-7 AM)
	if current_hour >= 5.0 and current_hour < 7.0:
		color = day_color.linear_interpolate(dawn_dusk_color, (current_hour - 5.0) / 2.0)
	# Day (7 AM - 7 PM)
	elif current_hour >= 7.0 and current_hour < 19.0:
		color = day_color
	# Dusk (7-9 PM)
	elif current_hour >= 19.0 and current_hour < 21.0:
		color = dawn_dusk_color.linear_interpolate(night_color, (current_hour - 19.0) / 2.0)
	# Night
	else:
		color = night_color
	
	canvas_modulate.color = color

func _update_weather(delta):
	weather_transition_time += delta
	if weather_transition_time >= weather_duration:
		_change_weather()
		weather_transition_time = 0.0

func _change_weather():
	var new_weather = weather_effects[randi() % weather_effects.size()]
	while new_weather == current_weather:
		new_weather = weather_effects[randi() % weather_effects.size()]
	current_weather = new_weather
	emit_signal("weather_changed", current_weather)
