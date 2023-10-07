import React, { Component } from 'react';
import FieldSet from 'Components/FieldSet';
import Link from 'Components/Link/Link';
import styles from '../styles.css';

class Donations extends Component {

  //
  // Render

  render() {
    return (
      <FieldSet legend="Donations">
        <div className={styles.logoContainer} title="Radarr">
          <Link to="https://opencollective.com/radarr">
            <img
              className={styles.logo}
              src={`${window.Speakarr.urlBase}/Content/Images/Icons/logo-radarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Lidarr">
          <Link to="https://opencollective.com/lidarr">
            <img
              className={styles.logo}
              src={`${window.Speakarr.urlBase}/Content/Images/Icons/logo-lidarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Speakarr">
          <Link to="https://opencollective.com/speakarr">
            <img
              className={styles.logo}
              src={`${window.Speakarr.urlBase}/Content/Images/Icons/logo-speakarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Prowlarr">
          <Link to="https://opencollective.com/prowlarr">
            <img
              className={styles.logo}
              src={`${window.Speakarr.urlBase}/Content/Images/Icons/logo-prowlarr.png`}
            />
          </Link>
        </div>
        <div className={styles.logoContainer} title="Sonarr">
          <Link to="https://opencollective.com/sonarr">
            <img
              className={styles.logo}
              src={`${window.Speakarr.urlBase}/Content/Images/Icons/logo-sonarr.png`}
            />
          </Link>
        </div>
      </FieldSet>
    );
  }
}

Donations.propTypes = {

};

export default Donations;
