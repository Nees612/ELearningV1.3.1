import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class VideosService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getAllVideos() {
    return this.http.get(environment.API_VIDEOS_URL + '/All', { headers: this.headersService.getHeaders() });
  }

  getVideosByModuleContent(id: number) {
    return this.http.get(environment.API_VIDEOS_URL + '/Video_by_module_content/' + id, { headers: this.headersService.getHeaders() });
  }

  addVideo(video) {
    return this.http.post(environment.API_VIDEOS_URL, video, { headers: this.headersService.getHeaders() });
  }

  deleteVideo(id) {
    return this.http.delete(environment.API_VIDEOS_URL + '/' + id, { headers: this.headersService.getHeaders() });
  }

}
