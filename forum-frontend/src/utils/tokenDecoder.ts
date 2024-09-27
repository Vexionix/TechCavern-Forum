import { jwtDecode } from "jwt-decode";


interface DecodedToken {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
    exp: number;
  }

  export default function decodeToken(accessToken:string) {
  const decodedToken: DecodedToken = jwtDecode<DecodedToken>(accessToken);
  const userId =
    decodedToken[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    ];
  const role =
    decodedToken[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

    return [userId, role];
}