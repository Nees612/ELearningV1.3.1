import { Component, OnInit, Input, Output, OnChanges } from '@angular/core';
import { VideosService } from '../services/videos.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { EventEmitter } from '@angular/core';
import { UsersService } from '../services/users.service';
import { IVideo } from '../Interfaces/IVideo';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit, OnChanges {

  @Input() contentId: number;

  @Output() playingVideo = new EventEmitter();
  @Output() videoClosed = new EventEmitter();

  showVideo: boolean = false;
  safeUrl: SafeResourceUrl;
  videos: IVideo[];

  hasVideo: boolean;
  isAdmin: boolean;

  constructor(private videosService: VideosService, private sanitizer: DomSanitizer, private usersService: UsersService) { }

  ngOnInit() {
    this.isAdmin = this.usersService.isAdmin();
    this.getContents()
  }

  ngOnChanges() {
    this.getContents();
  }

  private getContents() {
    this.videosService.getVideosByModuleContent(this.contentId).subscribe(response => {
      this.videos = response.json();
      if (this.videos.length > 0) {
        this.hasVideo = true;
      } else {
        this.hasVideo = false;
      }
    });
  }

  setBackGround(youtubeId) {
    let url = this.sanitizer.bypassSecurityTrustResourceUrl('https://img.youtube.com/vi/' + youtubeId + '/0.jpg');
    return url;
  }

  playVideo(url) {
    this.playingVideo.emit();
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    this.showVideo = true;
  }

  onVideoClosed() {
    this.videoClosed.emit();
    this.showVideo = false;
  }

  onDelete(id) {
    this.videosService.deleteVideo(id).subscribe(() => {
      this.getContents();
    });
    console.log(id);
  }

}
