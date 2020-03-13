import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-other-users-profile',
  templateUrl: './other-users-profile.component.html',
  styleUrls: ['./other-users-profile.component.css']
})
export class OtherUsersProfileComponent implements OnInit {

  selectedUser: any;

  constructor(private router: Router, private route: ActivatedRoute, private usersService: UsersService) { }

  ngOnInit() {
    let userName = this.route.snapshot.params['userName'];
    this.usersService.getUserData(userName).subscribe(response => {
      this.selectedUser = response.json().user;
    });
  }

  onCancel() {
    this.router.navigate(['/all_profiles']);
  }

}
