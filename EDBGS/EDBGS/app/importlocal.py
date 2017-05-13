
#Django code:
import json
def save_data(request):
  if request.method == 'POST':
    json_data = json.loads(request.body) # request.raw_post_data w/ Django < 1.4
    try:
      data = json_data['data']
    except KeyError:
      HttpResponseServerError("Malformed data!")
    HttpResponse("Got json data")


def save_events_json(request):
    if request.is_ajax():
        if request.method == 'POST':
            print 'Raw Data: "%s"' % request.body   
    return HttpResponse("OK")
