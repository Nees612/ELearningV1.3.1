import { Component, OnInit, Input, Output, OnChanges, Inject } from '@angular/core';
import { VideosService } from '../services/videos.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { EventEmitter } from '@angular/core';
import { UsersService } from '../services/users.service';
import { IVideo } from '../Interfaces/IVideo';
import { DOCUMENT } from '@angular/common';

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

  selectedVideo: IVideo;

  hasVideo: boolean;
  isAdmin: boolean;

  constructor(private videosService: VideosService, private sanitizer: DomSanitizer, private usersService: UsersService, @Inject(DOCUMENT) private _document) { }

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

  playVideo(video: IVideo) {
    this.selectedVideo = video;
    this.playingVideo.emit();
    //this._document.body.style.background = '#343a40';
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(video.url);
    this.showVideo = true;
  }

  onVideoClosed() {
    this.videoClosed.emit();
    this.selectedVideo = null;
    //this._document.body.style.background = '#FFF';
    this.showVideo = false;
  }

  onDelete(id) {
    this.videosService.deleteVideo(id).subscribe(() => {
      this.getContents();
    });
    console.log(id);
  }

}
