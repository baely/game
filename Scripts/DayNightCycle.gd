extends CanvasModulate

var time = 0
var day_duration = 300  # 5 minutes per day cycle
var start_color = Color(1.0, 1.0, 1.0, 1.0)  # Bright daylight
var night_color = Color(0.2, 0.2, 0.3, 1.0)  # Dark blue night
var sunrise_color = Color(1.0, 0.8, 0.5, 1.0)  # Warm sunrise
var sunset_color = Color(1.0, 0.6, 0.4, 1.0)  # Orange sunset
var wind_controller = null

func _ready():
	color = start_color
	wind_controller = get_node("../WindController")

func _process(delta):
	time += delta / day_duration
	
	# Create a day/night cycle
	var cycle = fmod(time, 1.0)
	
	if cycle < 0.25:  # Sunrise
		color = start_color.linear_interpolate(sunrise_color, cycle * 4)
		if wind_controller:
			# Calmer winds during sunrise
			var strength = lerp(0.1, 0.2, cycle * 4)
			wind_controller.wind_strength = lerp(wind_controller.wind_strength, strength, 0.1)
	elif cycle < 0.5:  # Day
		color = sunrise_color.linear_interpolate(start_color, (cycle - 0.25) * 4)
		if wind_controller:
			# Moderate winds during day
			var strength = lerp(0.2, 0.3, (cycle - 0.25) * 4)
			wind_controller.wind_strength = lerp(wind_controller.wind_strength, strength, 0.1)
	elif cycle < 0.75:  # Sunset
		color = start_color.linear_interpolate(sunset_color, (cycle - 0.5) * 4)
		if wind_controller:
			# Stronger winds during sunset
			var strength = lerp(0.3, 0.4, (cycle - 0.5) * 4)
			wind_controller.wind_strength = lerp(wind_controller.wind_strength, strength, 0.1)
	else:  # Night
		color = sunset_color.linear_interpolate(night_color, (cycle - 0.75) * 4)
		if wind_controller:
			# Variable winds at night
			var strength = 0.2 + sin(time * 10) * 0.2
			wind_controller.wind_strength = lerp(wind_controller.wind_strength, strength, 0.1)
